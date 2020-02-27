using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class DotNetPipeline
    {
        private readonly IPipelineBuilder _builder;

        // Output path and name
        private const string FILENAME = ".gitlab-ci.yml";
        private const string PATH = "/home/username/some/path";

        public DotNetPipeline()
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
                        Image("mcr.microsoft.com/dotnet/core/sdk").
                        Script("dotnet clean").
                    Stage("Build").
                        Job("Build").
                            Extends(".Clean").
                            Script("dotnet build").
                    Stage("Verify").
                        Job("Test").
                            Extends(".Clean").
                            Script("dotnet test --project GitLab_CI_DSL/").
                        Job("Run").
                            Extends(".Clean").
                            EnvVar("RunVars", "--project Pipelines/").            
                            Script("dotnet run $RunVars").
                Create();
        }
    }
}