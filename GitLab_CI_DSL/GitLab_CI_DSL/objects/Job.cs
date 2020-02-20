using System;
using System.Collections.Generic;

namespace GitLab_CI_DSL
{
   public class Job : DefaultJob
   {
       public string Name { get; set; }
       
       public string Stage { get; set; }
       public string Extension { get; set; }

       public Job(string name)
        {
            Name = name;
        }


   }
}