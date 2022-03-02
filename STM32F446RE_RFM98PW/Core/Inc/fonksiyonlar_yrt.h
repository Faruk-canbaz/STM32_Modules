
#include "LoRa.h"
#include "main.h"
LoRa myLoRa;

SPI_HandleTypeDef hspi2;
UART_HandleTypeDef huart2;

uint8_t size;
uint8_t Data[256];

uint16_t size_print;
uint8_t Data_print[256];

void loRaBaslat()
{
	myLoRa = newLoRa();
	HAL_Delay(5);

	myLoRa.hSPIx = &hspi2;
	myLoRa.CS_port = NSS_GPIO_Port;
	myLoRa.CS_pin = NSS_Pin;
	myLoRa.reset_port = RST_GPIO_Port;
	myLoRa.reset_pin = RST_Pin;
	myLoRa.DIO0_port = DIO0_GPIO_Port;
	myLoRa.DIO0_pin = DIO0_Pin;
	HAL_Delay(5);

	myLoRa.frequency = 434.0;             // default = 433 MHz
	myLoRa.spredingFactor = SF_9;            // default = SF_7
	myLoRa.bandWidth = BW_125KHz;       // default = BW_125KHz
	myLoRa.crcRate = CR_4_7;          // default = CR_4_5
	myLoRa.power = POWER_20db;      // default = 20db
	myLoRa.overCurrentProtection = 250; // default = 100 mA
	myLoRa.preamble = 8;              // default = 8;
	HAL_Delay(5);

	HAL_GPIO_WritePin(RST_GPIO_Port, RST_Pin, GPIO_PIN_SET);
	HAL_GPIO_WritePin(NSS_GPIO_Port, NSS_Pin, GPIO_PIN_SET);

	LoRa_reset(&myLoRa);
	uint16_t LoRa_status = LoRa_init(&myLoRa);

	if (LoRa_status == LORA_OK)
		printA("LoRa Cod: LORA_OK\n", 64);

	else if (LoRa_status == LORA_NOT_FOUND)
		printA("LoRa cod: LORA_NOT_FOUND\n", 64);

	else if (LoRa_status == LORA_UNAVAILABLE)
		printA("LoRa cod: LORA_UNAVAILABLE\n", 64);

	HAL_Delay(15);
}


void loraGonder(uint8_t* veri, uint16_t size)
{
	uint8_t ret = LoRa_transmit(&myLoRa, (uint8_t*) veri, size, 300);

	if (ret != 1)
		printA("LoRa Paketini Gonderirken Bir Sorun Oldu!\n", 64);
}

void printA(char* str, uint16_t size)
{
	uint8_t Data[256];
	size = sprintf((char *) Data, str);
	HAL_UART_Transmit(&huart2, Data, size, 1000);
}



