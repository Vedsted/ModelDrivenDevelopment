using GitLab_CI_DSL;

namespace MavenApp_CI
{
    public class MavenProjPipelineBuilder:PipelineBuilder
    {
        private const string FileName = ".gitlab-ci.yml";


        protected override void Build()
        {
            Pipeline(FileName).
                Default().
                    Image("maven:latest").
                    EnvVar("MAVEN_HOME", "/usr/share/mvn").
                    EnvVar("JAVA_HOME", "/usr/share/java").
                    Script("bash -c ./setup.sh").
                Extension(".build").
                    Script("mvn build").
                Stage("Verify").
                    Job("Compile").
                        Extend(".build").
                    Job("Test").
                        Extend(".build").
                        Script("mvn verify").
                Stage("Quality").
                    EnvVar("SonarURL", "http://my.sonar.path").
                    Job("SonarQube").
                        Extend(".build").
                        Script("mvn sonar:sonar");
        }
        
    }
}