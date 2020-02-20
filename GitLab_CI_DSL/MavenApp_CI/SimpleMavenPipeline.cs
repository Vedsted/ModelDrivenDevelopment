using GitLab_CI_DSL;

namespace MavenApp_CI
{
    public class SimpleMavenPipeline: PipelineBuilder
    {
        protected override void Build()
        {
            Pipeline("maven.yml")
                .Default()
                    .Image("maven:latest")
                .Extension(".build")
                    .Script("mvn clean build")
                .Stage("validation")
                    .Job("compile")
                        .Extend(".build")
                    .Job("test")
                        .Extend(".build")
                        .Script("mvn verify");
        }
    }
}