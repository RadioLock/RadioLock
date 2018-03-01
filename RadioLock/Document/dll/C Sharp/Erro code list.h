//Failed to open the port
#define Err_Dev_Port			-800

//Port has been Open
#define Err_Dev_Open			-801

//Port hasn't been Open
#define Err_Dev_NULLPort		-802

//Failed to close the port
#define Err_Dev_Port_STILL		-803

//COM size setting failed
#define Err_COM_SETSIZE		-811

//COM timeout setting failed
#define Err_COM_SETTIME		-812

//COM state setting failed
#define Err_COM_SETSTATE		-813

//Get COM state failed
#define Err_COM_GETSTATE		-814	

//Send data failed
#define Err_Dev_Send			-831
	
//Send data header failed
#define Err_Dev_SYNC			-832

//Send a byte data failed	
#define Err_Dev_W_BYTE		-833

//Receive a byte date failed
#define Err_Dev_R_BYTE		-834

//Receive data failed
#define Err_Dev_Rece			-835

//Receive packet head error
#define Err_Dev_Rece_Head		-836

//Receive packet tail error
#define Err_Dev_Rece_End		-837

//Receive packet size error
#define Err_Dev_Rece_Len		-838	

//Check receive packet error
#define Err_Dev_Rece_Chk		-839

//Receive packets not the machine
#define Err_Dev_Rece_Addr		-840	

//Receive empty data
#define Err_Dev_Rece_NULL		-841	

//Operated Dev failed
#define Err_Dev_Rece_Oper		-842	

//Send data timeout
#define Err_Dev_W_TIMEOUT		-843	

//Receive data timeout
#define Err_Dev_R_TIMEOUT		-844	

//Dev no answer
#define Err_Dev_NOANSWER		-845	

//Error Command
#define Err_Dev_CMD			-846	


//USB Device--------------------
//Send data error
#define Err_HID_Send			-601

//Receive data error
#define Err_HID_RECE			-602
//////////////////////////////


//Packet contains illegal characters
#define Err_Data			-1001

//Write card default check failed
#define Err_Write_DEF_CHK		-1101

//Write card data default check failed
#define Err_Write_DEF_ERR		-1102

//Write card data combo error
#define Err_Write_DEF_MAK		-1103

//Illegal card(not the card of the hotel)
#define Err_Read_CONFIG		-1201
	
//Read card default data failed
#define Err_Read_DEF			-1202
	
//Check card data failed
#define Err_Read_CHK			-1203	

        /// 
#define reg_UnRegistered  -2000

        /// 
#define reg_TimeOut  -2001