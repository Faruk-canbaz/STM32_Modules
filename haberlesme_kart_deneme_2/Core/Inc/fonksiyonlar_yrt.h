
#include "LoRa.h"

LoRa myLoRa;

uint16_t size_print;
uint8_t Data_print[256];

void loRaBaslat()
{
	myLoRa = newLoRa();
	HAL_Delay(5);

	  myLoRa.CS_port         = SPI1_NSS_GPIO_Port;
	  myLoRa.CS_pin          = SPI1_NSS_Pin;
	  myLoRa.reset_port      = SPI1_RST_GPIO_Port;
	  myLoRa.reset_pin       = SPI1_RST_Pin;
	  myLoRa.DIO0_port       = DIO0_GPIO_Port;
	  myLoRa.DIO0_pin        = DIO0_Pin;
	  myLoRa.hSPIx           = &hspi1;
	HAL_Delay(5);

	myLoRa.frequency = 434.0;             // default = 433 MHz
	myLoRa.spredingFactor = SF_9;            // default = SF_7
	myLoRa.bandWidth = BW_125KHz;       // default = BW_125KHz
	myLoRa.crcRate = CR_4_7;          // default = CR_4_5
	myLoRa.power = POWER_20db;      // default = 20db
	myLoRa.overCurrentProtection = 250; // default = 100 mA
	myLoRa.preamble = 8;              // default = 8;
	HAL_Delay(5);

	HAL_GPIO_WritePin(SPI1_RST_GPIO_Port, SPI1_RST_Pin, GPIO_PIN_SET);
	HAL_GPIO_WritePin(SPI1_NSS_GPIO_Port, SPI1_NSS_Pin, GPIO_PIN_SET);

	LoRa_reset(&myLoRa);
	uint16_t LoRa_status = LoRa_init(&myLoRa);

	if (LoRa_status == LORA_OK){

	}


	else if (LoRa_status == LORA_NOT_FOUND){

	}


	else if (LoRa_status == LORA_UNAVAILABLE){

	}

	HAL_Delay(15);
}


void loraGonder(uint8_t* veri, uint16_t size)
{
	uint8_t ret = LoRa_transmit(&myLoRa, (uint8_t*) veri, size, 100);

	if (ret != 1);
}



