using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class GitLabDslPipeline
    {
        private IPipelineBuilder _builder;

        private string FILENAME = ".gitlab-ci.yml";
        private string PATH = "~/";
        
        public GitLabDslPipeline()
        {
            _builder = new PipelineBuilder();

            var pipeline = CreatePipeline();
            
            new GitLabYmlGenerator().CreateGitlabCiConfig(FILENAME, PATH, pipeline);
        }

        private Pipeline CreatePipeline()
        {
            return _builder.
                Pipeline().
                    AbstractJob(".Clean").
                        Image("mcr.microsoft.com/dotnet/core/runtime").
                        Script("dotnet clean").
                    AbstractJob(".RunVars").
                        EnvVar("RunVars", "Pipelines/").
                    Stage("Build").
                        Job("Build").
                            Extends(".Clean").
                            Script("dotnet build").
                    Stage("Run").
                        Job("Run").
                            Extends(".Clean").
                            Extends(".RunVars").
                            Script("dotnet run $RunVars").
                Create();
        }
    }
}