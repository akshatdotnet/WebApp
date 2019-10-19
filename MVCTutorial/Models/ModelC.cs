using System.Collections.Generic;

namespace MVCTutorial.Models
{
    public class ModelC
    {
        public List<ModelA> ListA { get; set; }
        public List<ModelB> ListB { get; set; }
        public int Age { get; set; }
    }
}