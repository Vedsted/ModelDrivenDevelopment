using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class MavenPipeline
    {
        private IPipelineBuilder _builder;
        
        private const string FileName = ".gitlab-ci.yml";
        private const string PATH = "/home/username/some/path";
        
        public MavenPipeline()
        {
            _builder = new PipelineBuilder();

            var pipeline = CreatePipeline();
            
            new GitLabYmlGenerator().CreateGitlabCiConfig(FileName, PATH, pipeline);

        }

        private Pipeline CreatePipeline()
        {
            return _builder.
                Pipeline().
                    AbstractJob(".clean").
                        Image("maven:latest").
                        Script("mvn clean").
                    Stage("Build").
                        AbstractJob(".compile").
                            Extends(".clean").
                            Script("mvn compile").
                        Job("build").
                            Extends(".compile").
                            Script("echo \"Compiled Successfully!\"").
                    Stage("Test").
                        Job("test").
                            Extends(".clean").
                            Script("mvn verify").
                            Script("echo \"Tests passed!\"").
            Create();
        }
        
        
    }
}