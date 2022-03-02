
#include "bmp280.h"

BMP280_HandleTypedef bmp280_sensor;

uint16_t size_print;
uint8_t Data_print[256];

void startBmp(I2C_HandleTypeDef* i2c_port)
{
	bmp280_init_default_params(&bmp280_sensor.params);
	bmp280_sensor.addr = BMP280_I2C_ADDRESS_0;
	bmp280_sensor.i2c = i2c_port;

	while (!bmp280_init(&bmp280_sensor, &bmp280_sensor.params))
	{
		size_print = sprintf((char *) Data_print, "BMP280 initialization failed\n");
		HAL_UART_Transmit(&huart2, Data_print, size_print, 1000);
		HAL_Delay(2000);
	}

	bool bme280p = bmp280_sensor.id == BME280_CHIP_ID;

	size_print = sprintf((char *) Data_print, "BMP280: found %s\n",
			bme280p ? "BME280" : "BMP280");
	HAL_UART_Transmit(&huart2, Data_print, size_print, 1000);
}


void readBmp(float ret[2])
{
	float temperature, pressure, humidity;

	while (!bmp280_read_float(&bmp280_sensor, &temperature, &pressure, &humidity))
	{
		printA("Sicaklik/Basinc okurken bir hata olustu!\n", 64);
		HAL_Delay(2000);
	}

	ret[0] = temperature;
	ret[1] = pressure;

}

float calculateAltitude(float p, float pi)
{
	p = p / 100;
	pi = pi / 100;
	float alt = (44330 * (1.0 - pow(p / pi, 0.1903)));

	return alt;
}

void printA(char* str, uint16_t size)
{
	uint8_t Data[256];
	size = sprintf((char *) Data, str);
	HAL_UART_Transmit(&huart2, Data, size, 1000);
}






