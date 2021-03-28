using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class maxSharpness : MatrixFilter
    {
        public maxSharpness()
        {
            createSharpnessKernel();
        }
        protected void createSharpnessKernel()
        {
            kernel = new float[,]
                    { {-1, -1, -1 },
                      {-1,  9, -1 },
                      {-1, -1, -1 } };
        }
    };
}
