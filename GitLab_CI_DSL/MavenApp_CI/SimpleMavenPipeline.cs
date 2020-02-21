using GitLab_CI_DSL;

namespace MavenApp_CI
{
    public class SimpleMavenPipeline
    {
        private IPipelineBuilder _builder;
        
        private const string FilePath = "~/git/myproj";
        private const string FileName = ".gitlab-ci.yml";

        
        public SimpleMavenPipeline()
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
                    AbstractJob(".clean").
                        Script("mvn clean").
                    Stage("validation").
                        AbstractJob(".compile").
                            Extends(".clean").
                            Script("mvn compile").
                        Job("compile").
                            Extends(".compile").
                            Script("echo \"Compiled Successfully\"").
                        Job("test").
                            Extends(".compile").
                            Script("mvn verify").
                            Script("echo \"Unit tests passed\"").
            Create();
        }
    }
}