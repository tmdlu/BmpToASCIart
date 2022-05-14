using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace BmpToASCIIart.Application
{
    // BMP class, represent bmp files
    class BMP
    {
        //Load c++ dll
        [DllImport("C:/Users/ooo2t/source/repos/BmpToASCIIart/JACppDll/x64/Debug/JACppDll.dll")]
        
        public static extern char GrayToASCII2432Cpp (int b, int g, int r);
        [DllImport("C:/Users/ooo2t/source/repos/BmpToASCIIart/JACppDll/x64/Debug/JACppDll.dll")]
        public static extern char GrayToASCII16Cpp(int b, int g, int r);
       
        //Load asm dll
       [DllImport("C:/Users/ooo2t/source/repos/BmpToASCIIart/x64/Debug/JAAsm.dll")]
        static extern char gray2432funcASM(int b, int g, int r);
       [DllImport("C:/Users/ooo2t/source/repos/BmpToASCIIart/x64/Debug/JAAsm.dll")]
        static extern char gray16funcASM(int b, int g, int r);

        string name; //File name
        bmpInfo Info = new bmpInfo(); //Object with bmp informations
        int limit; //Setted amount of threads 
        List<List<char>> PixelArray = new List<List<char>>(); //two-dimensional list of ASCIIArt

        long time; //execute time 
        //Constructors
        public BMP(string name, int limit)
        {
            this.name = name;
            this.limit = limit;
        }

        public BMP() { }

        //Getters and Setters
        public long getTime()
        {
            return time;
        }

        public void setName(string name)
        {
            this.name = name;
        }
        public int getH()
        {
            return Info.getH();
        }
        public void setH(int height)
        {
            Info.setH(height);
        }
        public int getW()
        {
            return Info.getW();
        }
        public void setW(int width)
        {
            Info.setW(width);
        }

      
        public int getBpp()
        {
            return Info.getBpp();
        }
        public void setBpp(int bitsperPixel)
        {
            Info.setBpp(bitsperPixel);
        }

        //function connects value of two bytes to one value
        public int readSize(byte val1, byte val2)
        {

            int result = val2 << 8 | val1;
            return result;
        }

        //function changes color values of pixel to grayscale for 16 bpp files
        //in 2 bytes color is written
        public void bpp16(ref byte[] data, string choice)
        {
            List<List<char>> D = new List<List<char>>();

            int w = Info.getW() * 2;
            int h = Info.getH();
            int m = Info.getW() % 2;
            int num = data[14] + 14;
            byte[] buf = data;


            for (int i = 0; i < h; i++)
            {
                List<char> d = new List<char>(Info.getW());
                for (int j = 0; j < Info.getW(); j++)
                    d.Add(' ');
                D.Add(d);
            }
            PixelArray = D;
            


            if(choice=="asm")
            {
                Parallel.For(0, h, new ParallelOptions { MaxDegreeOfParallelism = limit },
             i => {
                 int k = i * (w + 2 * m);


                 for (int j = 0; j < Info.getW(); j++)
                 {

                     BitArray ba1 = new BitArray(BitConverter.GetBytes(buf[(num + k + j * 2)]).ToArray());
                     BitArray ba2 = new BitArray(BitConverter.GetBytes(buf[(num + k + j * 2) + 1]).ToArray());

                     //seperating color componets
                     BitArray r = new BitArray(5);
                     BitArray g = new BitArray(6);
                     BitArray b = new BitArray(5);
                     for (int l = 0; l < 5; l++)
                     {
                         b[l] = ba1[l];
                         r[l] = ba2[l + 3];
                     }
                     g[0] = ba1[5];
                     g[1] = ba1[6];
                     g[2] = ba1[7];
                     g[3] = ba2[0];
                     g[4] = ba2[1];
                     g[5] = ba2[2];

                     int[] rgb = new int[3];
                     r.CopyTo(rgb, 2);
                     g.CopyTo(rgb, 1);
                     b.CopyTo(rgb, 0);
                     PixelArray[i][j] = gray16funcASM(rgb[0], rgb[1], rgb[2]);




                 }
             });


            }
            else if (choice=="cpp")
            {
                Parallel.For(0, h, new ParallelOptions { MaxDegreeOfParallelism = limit },
             i => {
                 int k = i * (w + 2 * m);


                 for (int j = 0; j < Info.getW(); j++)
                 {

                     BitArray ba1 = new BitArray(BitConverter.GetBytes(buf[(num + k + j * 2)]).ToArray());
                     BitArray ba2 = new BitArray(BitConverter.GetBytes(buf[(num + k + j * 2) + 1]).ToArray());
                     
                     //seperating color componets
                     BitArray r = new BitArray(5);
                     BitArray g = new BitArray(6);
                     BitArray b = new BitArray(5);
                     for (int l = 0; l < 5; l++)
                     {
                         b[l] = ba1[l];
                         r[l] = ba2[l + 3];
                     }
                     g[0] = ba1[5];
                     g[1] = ba1[6];
                     g[2] = ba1[7];
                     g[3] = ba2[0];
                     g[4] = ba2[1];
                     g[5] = ba2[2];

                     int[] rgb = new int[3];
                     r.CopyTo(rgb, 2);
                     g.CopyTo(rgb, 1);
                     b.CopyTo(rgb, 0);
                     PixelArray[i][j] = GrayToASCII16Cpp(rgb[0], rgb[1], rgb[2]);
                 }
             });
            }
       
        }


        //function changes color values of pixel to grayscale for 24/32 bpp files
        //in 3 bytes color is written
        public int bpp2432(ref byte[] data, int x, string choice)
        {
            List<List<char>> D = new List<List<char>>();
            byte[] buf = data;

            int w = Info.getW() * x;
            int h = Info.getH();
            int m = Info.getW() % 4; //amount of bytes, which must be aligned
            int num = data[14] + 14;
            int size = data.Length;
          
            for (int i = 0; i < h; i++)
            {
                List<char> d = new List<char>(Info.getW());
                for (int j = 0; j < Info.getW(); j++)
                    d.Add(' ');
                D.Add(d);
            }
            PixelArray = D;



            if(choice=="asm")
            {
                Parallel.For(0, h, new ParallelOptions { MaxDegreeOfParallelism = limit },
              i =>
              {
                  int k = i * (w + m);
                  for (int j = 0; j < Info.getW(); j++)
                  {

                      PixelArray[i][j] = gray2432funcASM(buf[num + k], buf[num + k + 1], buf[num + k + 2]);

                      k += x;
                  }
              }
              );
            }
            else if (choice=="cpp")
            {
                Parallel.For(0, h, new ParallelOptions { MaxDegreeOfParallelism = limit },
              i =>
              {
                  int k = i * (w + m);
                  for (int j = 0; j < Info.getW(); j++)
                  {

                      PixelArray[i][j] = GrayToASCII2432Cpp(buf[num + k], buf[num + k + 1], buf[num + k + 2]);
                        
                        k += x;
                  }
              }
              );
            }
            return 0;
        }

        //function returns string array of ASCII art
        public string[] getASCIIart()
        {
            int l = 0;
            string[] result = new string[Info.getH()];
            for (int i = Info.getH() - 1; i >= 0; i--) //the image must be displayed in reverse
            {
                string x = "";

                
                    for (int j = 0; j < Info.getW(); j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            x += PixelArray[i][j];

                        }

                    }

                
                result[l] = x;

                l++;

            }
            File.WriteAllLines("test.txt", result);
            return result;



        }
        //function controls action of program
        public string[] readBMP(ref string output, string choice)
        {

            byte[] data = null;
            FileStream file = new FileStream(name, FileMode.Open, FileAccess.Read);

            BinaryReader reader = new BinaryReader(file);

            long numBytes = new FileInfo(name).Length;
            data = reader.ReadBytes((int)numBytes); //reads bytes of file
            Info.setW(readSize(data[18], data[19])); //set width of image
            Info.setH(readSize(data[22], data[23])); //set height of image
            Info.setBpp(data[28]); //set color depth


            Stopwatch sw = new Stopwatch();
            sw.Start(); //start measure time
            if (Info.getBpp() == 8)
            {
                output = "The program does't operate on this type of files";

            }
            else if (Info.getBpp() == 16)
            {
                output = "File 16";
                bpp16(ref data, choice);
            }
            else if (Info.getBpp() == 24)
            {
                output = "File 24";
                bpp2432(ref data, 3, choice);

            }
            else if (Info.getBpp() == 32)
            {
                output = "File 32";
                bpp2432(ref data, 4, choice);
            }
            sw.Stop(); //stop measure time
            time = sw.ElapsedMilliseconds; //time in milliseconds
            string[] lines = getASCIIart();
            return lines;
            
        }


    }
}
