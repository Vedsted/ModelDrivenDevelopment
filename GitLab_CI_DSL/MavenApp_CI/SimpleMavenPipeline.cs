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
                .Extension(".clean")
                    .Script("mvn clean")
                .Stage("validation")
                    .Extension(".compile")
                        .Extend(".clean")
                        .Script("mvn compile")
                    .Job("compile")
                        .Extend(".compile")
                        .Script("echo \"Compiled Successfully\"")
                    .Job("test")
                        .Extend(".compile")
                        .Script("mvn verify")
                        .Script("echo \"Unit tests passed\"");
        }
    }
}