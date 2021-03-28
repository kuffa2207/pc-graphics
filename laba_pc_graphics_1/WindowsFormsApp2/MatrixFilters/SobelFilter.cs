using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class SobelFilter : BorderAllocation
    {
        public SobelFilter()
        {
            createSobelKernel();
        }
        protected void createSobelKernel()
        {
            kernel = new float[,]
                {{-1, 0, 1 },
                 {-2, 0, 2 },
                 {-1, 0, 1 }};

        }
        protected override void changeFilterKernel(int ch)
        {
            if (ch == 1)
            {
                kernel = new float[,]
                       {{-1, 0, 1 },
                        {-2, 0, 2 },
                        {-1, 0, 1 }};
            }
            if (ch == 2)
            {
                kernel = new float[,]
                        {{-1, -2, -1 },
                         { 0,  0,  0 },
                         { 1,  2,  1 }};
            }
        }
    };
}
