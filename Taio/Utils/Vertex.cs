using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Utils
{
    struct Vertex
    {
        public int index;
        public int inDeg;
        public int outDeg;

        public Vertex(int index, int inDeg, int outDeg)
        {
            this.index = index;
            this.inDeg = inDeg;
            this.outDeg = outDeg;
        }
        public Vertex(int index)
        {
            this.index = index;
            this.inDeg = 0;
            this.outDeg = 0;
        }
        public int GetValue()
        {
            return this.outDeg - this.inDeg;
        }
    }
}
