
#define BUZZER_PIN 3
#define TRIGGER_PIN 13
#define ECHO_PIN 12
#define LED_1_PIN 10
#define LED_2_PIN 9
#define LED_3_PIN 8
#define BUTTON_PIN A0
#define PRESSURE_PIN A1

const int pressureZero = 102.4; //analog reading of pressure transducer at 0psi
const int pressureMax = 921.6; //analog reading of pressure transducer at 30psi
const int pressuretransducermaxPSI = 30; //psi value of transducer being used

float distance;
float duration;


int last_pin_used = LED_BUILTIN;
unsigned long current_time=0; 
unsigned long tone_duration = 300;
unsigned long last_time_buzzed = 0;

unsigned long duration_for_next_state = 0;
unsigned long time_since_previous_state = 0;

unsigned long current_time_micros;
unsigned long duration_micro_for_next_state = 0;
unsigned long time_micro_since_previous_state = 0;

float pressureBtnValue = 0;
bool pressureBtnIsDown = false;

float lakeBtnValue = 0;
bool lakeBtnIsDown = false;
bool lakeBtnPressed = false;

unsigned long last_time_msg_sent =0;

enum Sensor_State {
  START_SEND,
  STOP_SEND,
  START_RECEIVE,
  STOP_RECEIVE
}; 
Sensor_State current_sensor_state = START_SEND;



void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(TRIGGER_PIN,OUTPUT);
  pinMode(ECHO_PIN, INPUT); 
  /*pinMode(LED_BUILTIN, OUTPUT);
  pinMode(LED_1_PIN, OUTPUT);
  pinMode(LED_2_PIN, OUTPUT);
  pinMode(LED_3_PIN, OUTPUT);*/

   //Init btn
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  pinMode(PRESSURE_PIN, INPUT_PULLUP);
  //pressureBtnPrevValue = analogRead(BUTTON_PIN);
  //pressureBtnValue = analogRead(BUTTON_PIN);


}
void loop() {
  current_time = millis();
  current_time_micros = micros();
 

// arbitrary delay to limit data to serial port 
  distanceSensorTest();
  testPressureBtn();
  testLakeButton();
  sendMsg();
  //delay(100);
}
 long val =0;
void testLakeButton(){
   // Read pushbutton
  lakeBtnValue = analogRead(BUTTON_PIN);
  lakeBtnPressed= false;
  if (lakeBtnValue < 200 && !lakeBtnIsDown){
    lakeBtnPressed= true;
    lakeBtnIsDown = true;
    //triggerLed(LED_BUILTIN);
  }
  else if(lakeBtnValue > 500)
  {
    lakeBtnIsDown = false;
  }
 
}

void distanceSensorTest(){

  // put your main code here, to run repeatedly:
  if(current_sensor_state == START_SEND){
    digitalWrite(TRIGGER_PIN, LOW);
    
   // Wait for 2 microseconds
   time_micro_since_previous_state = current_time_micros;
    duration_micro_for_next_state = 2;
    current_sensor_state = STOP_SEND;
  }
  if(current_sensor_state == STOP_SEND and isDelayMicroTimePassed()){

 
    digitalWrite(TRIGGER_PIN, HIGH);
   // Wait for 10 microseconds
    time_micro_since_previous_state = current_time_micros;
    duration_micro_for_next_state = 10;
    current_sensor_state = START_RECEIVE;
  }
 
  if( current_sensor_state == START_RECEIVE and isDelayMicroTimePassed()){
    digitalWrite(TRIGGER_PIN, LOW);
    duration=pulseIn(ECHO_PIN, HIGH);
    distance=(duration*0.034)/2;
  
   /* if(distance > 15 and distance < 25){
      triggerLed(LED_1_PIN);
    }
    if(distance > 40 and distance < 50){
      triggerLed(LED_2_PIN);
    }
    if(distance > 65 and distance < 75){
      triggerLed(LED_3_PIN);
    }
    turnOffLed();*/
    current_sensor_state = STOP_RECEIVE;
    time_since_previous_state = current_time;
    duration_for_next_state = 100;
  }
  
 
  if(current_sensor_state == STOP_RECEIVE and isDelayTimePassed() )
  {
    current_sensor_state = START_SEND;
  }

}

bool isDelayTimePassed(){

  return ((time_since_previous_state + duration_for_next_state)<current_time);
}

bool isDelayMicroTimePassed(){

  return ((time_micro_since_previous_state + duration_micro_for_next_state)<current_time_micros);
}



void testPressureBtn() {
  // Read pushbutton
  pressureBtnValue = analogRead(PRESSURE_PIN);
  pressureBtnValue = ((pressureBtnValue-pressureZero)*pressuretransducermaxPSI)/(pressureMax-pressureZero); //conversion equation to convert analog reading to psi
  pressureBtnIsDown= false;
  if(pressureBtnValue > 0.2){
    pressureBtnIsDown = true;
  }
}

void sendMsg(){
  if(last_time_msg_sent + 100 < current_time){
    int pressureBtnPress = pressureBtnIsDown?1:0;
    int lakeBtnPress = lakeBtnIsDown?1:0;
    String msg = (String)distance +";"+ (String)pressureBtnValue+";"+ (String)pressureBtnPress +";" +(String)lakeBtnPress + ";";
    Serial.println(msg);
    //Serial.println(val);
    last_time_msg_sent = current_time;
 }
 
}