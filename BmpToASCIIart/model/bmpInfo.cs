using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpToASCIIart.Application
{
    class bmpInfo
    {
        int height;//height of image
        int width; //width of image
      
        int bitsperPixel; //color depth, number of bits per Pixel

        //Constructors
        public bmpInfo(int height, int width, int bitsperPixel)
        {
            this.height = height;
            this.width = width;
      
            this.bitsperPixel = bitsperPixel;
        }

        public bmpInfo() { }

        //getters and setters
        public int getH()
        {
            return height;
        }
        public void setH(int height)
        {
            this.height = height;
        }
        public int getW()
        {
            return width;
        }
        public void setW(int width)
        {
            this.width = width;
        }
       
      
        public int getBpp()
        {
            return bitsperPixel;
        }
        public void setBpp(int bitsperPixel)
        {
            this.bitsperPixel = bitsperPixel;
        }
    }
}
