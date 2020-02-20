using GitLab_CI_DSL;

namespace MavenApp_CI
{
    public class MavenProjFluentPipeline:FluentPipeline
    {
        private string fileName = ".gitlab-ci.yml";
        
        
        public override void Build()
        {
            Pipeline(".gitlab-ci.yml").
                Deafault().
                    Image("maven:latest").
                    EnvVar("MAVEN_HOME", "/usr/share/mvn").
                    EnvVar("JAVA_HOME", "/usr/share/java").
                    Script("before", "bash -c ./setup.sh").
                    Script("after", "bash -c ./cleanup.sh").
                Stage("Verify").
                    Job("Compile").
                        Script("mvn install -DskipTests").
                        Script("bash -c ./myScript.sh").
                    Job("Test").
                        Rule().
                        Script("mvn verify").
                        Artifact("JAR", "./target/*.jar").
                Stage("Quality").
                    EnvVar("SonarURL", "http://my.sonar.path").
                    Job("SonarQube").
                    Script("mvn sonar:sonar").
            Build();
        }
        
    }
}