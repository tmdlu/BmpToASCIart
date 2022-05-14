#include "pch.h"
#include "functions.h"



functions::functions()
{
	
}



char functions::GrayToASCIIfunc(double val)
{
	char result;
	if (val > 0.67)
	{
		result = 65 + (val - 0.67) * 77;
	}

	else if (val >= 0.34)
		result = 97 + (val - 0.34) * 78;
	else
		result = 32 + val * 46;

	return result;
}


double functions::gray2432func(int b, int g, int r)
{
	return ((0.11 * b + 0.59 * g + 0.3 * r) / 255);
}

double functions::gray16func(int b, int g, int r)
{
	return (0.11 * b + 0.3 * r) / 32 + (0.59 * g)/63;
}