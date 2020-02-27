using System;
using System.Collections.Generic;
using GitLab_CI_DSL.metamodel.@default;
using GitLab_CI_DSL.metamodel.jobs;
using GitLab_CI_DSL.metamodel.pipeline;
using GitLab_CI_DSL.metamodel.stage;

namespace GitLab_CI_DSL.Builder
{
    public class PipelineBuilder: IPipelineBuilder
    {
        private Pipeline _pipeline;
        
        private Default _currentDefault;

        private Stage _currentStage;

        private Job _currentJob;
        

        public Pipeline Create()
        {
            SaveCurrentStage();
            SaveDefault();
            return _pipeline;
        }
        
         public PipelineBuilder Pipeline()
        {
            if (_pipeline != null)
                throw new Exception("Only one 'Pipeline' is allowed");
            
            _pipeline= new Pipeline();
            return this;
        }

        public PipelineBuilder Default()
        {
            if (_currentDefault != null || _pipeline.Default != null)
            {
                throw new Exception("Multiple instances of 'Default' is not supported");
            }

            if (_currentStage != null || _currentJob != null)
            {
                throw new Exception("Default must be defined before: Stages, Jobs and AbstractJobs");
            }

            _currentDefault = new Default();
            return this;
        }
        
        public PipelineBuilder AbstractJob(string abstractJobName)
        {
            SaveDefault();
            SaveCurrentJob();
            _currentJob = new Job(abstractJobName, JobType.ABSTRACTJOB);
            return this;
        }
        
        public PipelineBuilder Stage(string name)
        {
            SaveDefault();
            SaveCurrentJob();
            SaveCurrentStage();
            _currentStage = new Stage(name);
            
            return this;
        }
        
        public PipelineBuilder Job(string name)
        {
            if (_currentStage == null)
            {
                throw new Exception("Job '" + name + "' can only be set for a stage.");
            }
            
            SaveCurrentJob();
            _currentJob = new Job(name, JobType.JOB);
            return this;
        }

        public PipelineBuilder Image(string imageName)
        {
            if (_currentDefault != null)
            {
                _currentDefault.SetImage(imageName);
                return this;
            }

            if (_currentJob != null)
            {
                _currentJob.SetImage(imageName);
                return this;
            }
            
            throw new Exception($"No active Default, Job or Abstract Job. Image '{imageName}' can not be applied to nothing.");
        }

        public PipelineBuilder EnvVar(string key, string value)
        {
            if (_currentDefault != null)
            {
                _currentDefault.AddEnvironmentVariable(key, value);
                return this;
            }

            if (_currentJob != null)
            {
                _currentJob.AddEnvironmentVariable(key, value);
                return this;
            }
            
            throw new Exception($"No active Default, Job or Abstract Job. EnVar with key: '{key}' and value: '{value}' can not be applied to nothing.");
        }

        public PipelineBuilder Script(string command)
        {
            if (_currentJob == null)
            {
                throw new Exception($"Error for Script: '{command}'.\nScript can only be applied for Jobs and Abstract Jobs.");
            }
            _currentJob.AddScript(command);
            return this;
        }

        public PipelineBuilder Extends(string abstractJobName)
        {
            if (_currentJob == null)
                throw new Exception($"Error for Extends: '{abstractJobName}'.\nExtends can only be applied for Jobs and Abstract Jobs.");
            
            
            var abstractJobs = new List<Job>();
            
            if (_pipeline.AbstractJobs != null)
                abstractJobs.AddRange(_pipeline.AbstractJobs);
            if(_currentStage != null && _currentStage.Jobs != null)
                abstractJobs.AddRange(_currentStage.Jobs);
            
            var absJob = abstractJobs.Find(j => j.Name == abstractJobName);

            if (absJob == null)
                throw new Exception($"Extends: '{abstractJobName}' does not exist!");
            
            _currentJob.AddExtension(absJob);
            return this;
        }


        private void SaveCurrentJob()
        {
            if (_currentJob == null)
                return;

            if (_currentStage == null)
                _pipeline.AddAbstractJob(_currentJob);
            else
                _currentStage.AddJob(_currentJob);

            _currentJob = null;
        }
        
        private void SaveCurrentStage()
        {
            if (_currentStage == null) return;
            
            SaveCurrentJob();
            _pipeline.AddStage(_currentStage);
            _currentStage = null;
        }

        /**
         * Saves the Default job if one exists.
         * Only one Default job is permitted per pipeline.
         */
        private void SaveDefault()
        {
            if (_currentDefault == null) return;
            
            _pipeline.SetDefault(_currentDefault);
            _currentDefault = null;
        }
    }
}