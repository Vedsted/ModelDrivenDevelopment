using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class InheritancePipeline
    {
        private readonly IPipelineBuilder _builder;

        // Output path and name
        private const string FILENAME = ".gitlab-ci.yml";
        private const string PATH = "/home/username/some/path";

        public InheritancePipeline()
        {
            _builder = new PipelineBuilder();
            var pipeline = CreatePipeline();
            new GitLabYmlGenerator().CreateGitlabCiConfig(FILENAME, PATH, pipeline);
        }

        private Pipeline CreatePipeline()
        {
            return _builder.
                Pipeline().  
                    AbstractJob(".abs1").
                        Image("abstractImage").
                        EnvVar("ToPrint", "abstract 1").
                    AbstractJob(".abs2").
                        Extends(".abs1").
                        Image("abstractImage 2").
                        EnvVar("ToPrint", "abstract 2").
                    AbstractJob(".abs3").
                        Image("abstractImage 3").
                        EnvVar("ToPrint", "abstract 3").
                    Stage("Print").
                        Job("Print").
                            Extends(".abs2").
                            Extends(".abs3").
                            Image("JobImage").
                            Script("echo $ToPrint").
                Create();
        }
    }
}