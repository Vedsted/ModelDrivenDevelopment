using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public abstract class PipelineBuilder
    {
        private Pipeline _pipeline;
        
        private Default _default;

        //private List<Stage> _stages;
        private Stage _currentStage;

        //private List<Extension> _extensions;
        private Job _currentExtension;
        
        //private List<Job> _jobs;
        private Job _currentJob;


        protected PipelineBuilder()
        {
            Build();
            SaveExistingJob();
            SaveExistingStage();
            SaveExistingExtension();
            SaveDefault();
            new GitLabParser().CreateGitlabCiConfig(_pipeline);
        }


        protected abstract void Build();
        
        /**
         * Initializes the Pipeline with a given name.
         */
         protected PipelineBuilder Pipeline(string name)
        {
            _pipeline= new Pipeline(name);
            //_stages = new List<Stage>();
            //_extensions = new List<Extension>();
            //_jobs = new List<Job>();

            return this;
        }

        /**
         * Creates a default job applying Image, Environment-Variables and Scripts for all other jobs in the pipeline.
         */
        public PipelineBuilder Default()
        {
            if (_default != null || _pipeline.Default != null)
            {
                throw new Exception("Multiple instances of 'Default' is not supported");
            }

            if (_currentStage != null || _currentExtension != null || _currentJob != null)
            {
                throw new Exception("Default must be defined before: Stages, Jobs and Extensions");
            }

            _default = new Default();
            return this;
        }

        /**
         * Creates an Extension job which can be inherited from other jobs. 
         * The Extension job will not be executed as a job in it self.
         */
        public PipelineBuilder Extension(string name)
        {
            SaveDefault();
            SaveExistingJob();
            SaveExistingExtension();
            _currentExtension = new Job(name);
            
            return this;
        }
        

        /**
         * Creates a Stage where multiple jobs can be created.
         */
        public PipelineBuilder Stage(string name)
        {
            SaveDefault();
            SaveExistingExtension();
            SaveExistingJob();
            SaveExistingStage();
            _currentStage = new Stage(name);
            
            return this;
        }


        /**
         * Creates a Job where  
         */
        public PipelineBuilder Job(string name)
        {
            if (_currentStage == null)
            {
                throw new Exception("Job '" + name + "' can only be set for a stage.");
            }
            
            SaveExistingJob();
            _currentJob = new Job(name);
            return this;
        }

        public PipelineBuilder Image(string imageName)
        {
            var job = GetActiveJob("Image");
            job.Image = imageName;
            return this;
        }

        public PipelineBuilder EnvVar(string key, string value)
        {
            var job = GetActiveJob("EnvVar");
            job.AddEnvironmentVariable(key, value);
            return this;
        }

        public PipelineBuilder Script(string command)
        {
            var job = GetActiveJob("Script");
            job.AddScript(command);
            return this;
        }

        public PipelineBuilder Extend(string extensionName)
        {
            if (_currentJob == null)
            {
                throw new Exception("Model invalid at Extend: '"+ extensionName+"'\nExtend can only be used for Jobs");
            }

            //sions.Find(e => e.Name == extensionName);
            _currentJob.Extension = extensionName;
            return this;
        }


        private void SaveExistingExtension()
        {
            if (_currentExtension != null)
            {
                _pipeline.AddExtension(_currentExtension);
                _currentExtension = null;
            }
        }
        private void SaveExistingJob()
        {
            if (_currentJob != null)
            {
                _currentJob.Stage = _currentStage.Name;
                _pipeline.AddJob(_currentJob);
                _currentJob = null;
            }
        }
        private void SaveExistingStage()
        {
            if (_currentStage != null)
            {
                _pipeline.AddStage(_currentStage);
                _currentStage = null;
            }
        }
        
        /**
         * Helper method for getting the active job (Default, Extension or Job)
         */
        private Default GetActiveJob(string action)
        {
            if (_currentJob != null)
            {
                return _currentJob;
            }

            if (_currentExtension != null)
            {
                return _currentExtension;
            }

            if (_default != null)
            {
                return _default;
            }
            
            throw new Exception("No active job found!\n" + action + " can only be attached to: Jobs, Extension and Default.");
        }
        
        /**
         * Saves the Default job if one exists.
         * Only one Default job is permitted per pipeline.
         */
        private void SaveDefault()
        {
            if (_default != null && _pipeline.Default == null)
            {
                _pipeline.Default = _default;
                _default = null;
            }
        }
    }
}