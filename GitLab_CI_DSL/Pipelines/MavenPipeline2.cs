using GitLab_CI_DSL.Builder;
using GitLab_CI_DSL.Generator;
using GitLab_CI_DSL.metamodel.pipeline;

namespace Pipelines
{
    public class MavenPipeline2
    {
        private IPipelineBuilder _builder;
        
        private const string FileName = ".gitlab-ci.yml";
        private const string FilePath = "/home/username/some/path";
        
        public MavenPipeline2()
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
                        Image("cfeicommon/maven-jdk8-fx").
                        EnvVar("MAVEN_M2", "-Dmaven.repo.local=.m2").
                        EnvVar("MAVEN_OPTIONS", "--batch-mode").
                    Stage("Build").
                        Job("maven_compile").
                            Script("mvn $MAVEN_M2 $MAVEN_OPTIONS clean install -DskipTests").
                    Stage("Test").
                        Job("maven_test").
                            Script("mvn $MAVEN_M2 $MAVEN_OPTIONS verify").
                    Stage("Quality").
                        Job("sonarqube").
                            EnvVar("SONAR_PROJKEY", "-Dsonar.projectKey=dk.sdu.mmmi:my-project").
                            EnvVar("SONAR_HOSTURL", "http://my.sonar.path").
                            EnvVar("SONAR_LOGIN", "-Dsonar.login=123456789").
                            Script("mvn $MAVEN_M2 $MAVEN_OPTIONS sonar:sonar $SONAR_PROJKEY $SONAR_HOSTURL $SONAR_LOGIN").
                    Stage("Documentation").
                        Job("javadoc").
                            Script("mvn $MAVEN_M2 $MAVEN_OPTIONS javadoc:aggregate").
                            Script("tar -cf docs.tar target/site/apidocs").
                            Script("Echo 'Push files to docs server'").
                Create();
        }
    }
}