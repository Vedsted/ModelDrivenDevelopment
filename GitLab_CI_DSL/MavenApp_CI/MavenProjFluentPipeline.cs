using GitLab_CI_DSL;

namespace MavenApp_CI
{
    public class MavenProjPipeline
    {
        private IPipelineBuilder _builder;
        
        private const string FilePath = "~/git/myproj";
        private const string FileName = ".gitlab-ci.yml";

        
        public MavenProjPipeline()
        {
            _builder = new PipelineBuilder();

            var pipeline = CreatePipeline();
            
            new GitLabYmlGenerator().CreateGitlabCiConfig(FileName, FilePath, pipeline);

        }

        private Pipeline CreatePipeline()
        {
            return _builder.
                Pipeline().
                    Default().
                        Image("maven:latest").
                        EnvVar("MAVEN_HOME", "/usr/share/mvn").
                        EnvVar("JAVA_HOME", "/usr/share/java").
                    AbstractJob(".build").
                        Script("mvn build").
                    Stage("Verify").
                        Job("Compile").
                            Extends(".build").
                        Job("Test").
                            Extends(".build").
                            Script("mvn verify").
                    Stage("Quality").
                        Job("SonarQube").
                            Extends(".build").
                            EnvVar("SonarURL", "http://my.sonar.path").
                            Script("mvn sonar:sonar").
                Create();
        }
    }
}