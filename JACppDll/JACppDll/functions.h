
class functions
{

public:
	
	functions();
	char GrayToASCIIfunc(double val); //function returns character, which represents value of grayscale
	double gray2432func(int b, int g, int r); //function returns gray value for 24bpp and 32 bpp files
	double gray16func(int b, int g, int r); //function returns gray value for 16 files
};



extern "C" __declspec(dllexport) char GrayToASCII2432Cpp(int b, int g, int r) // function, which  is called in main project, returns ASCII for 24bpp and 32bpp files
{
	functions func;
	return func.GrayToASCIIfunc(func.gray2432func(b, g, r));
}
extern "C" __declspec(dllexport) char GrayToASCII16Cpp(int b, int g, int r) // function, which  is called in main project, returns ASCII for 16bpp files
{
	functions func;
	return func.GrayToASCIIfunc(func.gray16func(b, g, r));
}

