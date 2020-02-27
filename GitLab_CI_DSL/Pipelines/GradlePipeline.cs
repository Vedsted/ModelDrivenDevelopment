using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class GradlePipeline
    {
        private readonly IPipelineBuilder _builder;

        // Output path and name
        private const string FILENAME = ".gitlab-ci.yml";
        private const string PATH = "/home/username/some/path";

        public GradlePipeline()
        {
            _builder = new PipelineBuilder();
            var pipeline = CreatePipeline();
            new GitLabYmlGenerator().CreateGitlabCiConfig(FILENAME, PATH, pipeline);
        }

        private Pipeline CreatePipeline()
        {
            return _builder.
                Pipeline().
                    AbstractJob(".SetUp").
                        Image("gradle:alpine").
                        Script("export GRADLE_USER_HOME=`pwd`/.gradle").
                    Stage("Build").
                        Job("Build").
                            Extends(".SetUp").
                            Script("gradle --build-cache assemble").
                    Stage("Test").
                        Job("Test").
                            Extends(".SetUp").
                            Script("gradle check").
                Create();
        }
    }
}