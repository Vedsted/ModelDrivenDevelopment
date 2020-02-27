using GitLab_CI_DSL.metamodel.pipeline;

namespace GitLab_CI_DSL.Builder
{
    public interface IPipelineBuilder
    {
        /**
        * Finalizes the pipeline creation and returns the pipeline.
        */
        public Pipeline Create();

        /**
         * Initializes the Pipeline.
         */
        public PipelineBuilder Pipeline();

        /**
         * Creates a default configuration applied for all jobs in the pipeline.
         */
        public PipelineBuilder Default();

        /**
         * Creates an Abstract Job.
         * Abstract Jobs can be inherited from other Jobs or Abstract Jobs.
         * The Abstract Job will not be executed as a job in it self, but serves as the purpose of reuse.
         * 
         * Abstract jobs can be created out of a Stage.
         * If defined within a Stage, all Jobs and Abstract Jobs extending it will be part of the Stage.
         */
        public PipelineBuilder AbstractJob(string abstractJobName);

        /**
         * Creates a Stage where Jobs and Abstract Jobs can be defined.
         */
        public PipelineBuilder Stage(string name);

        /**
         * Creates a Job in the current Stage.
         */
        public PipelineBuilder Job(string name);

        /**
         * Adds a Docker image to active: Default, Job or Abstract Job.
         */
        public PipelineBuilder Image(string imageName);

        /**
         * Adds a environment variable to the active: Default, Job or Abstract Job.
         */
        public PipelineBuilder EnvVar(string key, string value);

        /**
         * Adds a script to the current Job or Abstract Job
         */
        public PipelineBuilder Script(string command);

        /**
         * Defines that the current job extends the given AbstractJob.
         * It is only possible to extend an already existing Abstract Job.
         */
        public PipelineBuilder Extends(string abstractJobName);
    }
}