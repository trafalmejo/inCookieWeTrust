#include <Servo.h>

Servo myservo;  // create servo object to control a servo
//PIN CAPACITIVE SENSOR
int capacitiveSensorPinInput = 2;
//CAPACITIVE VALUE
int capacitiveSensorValue = 0;
//LAST CAPACITIVE VALUE
int lastCapacitiveSensorValue = 0;

void setup() {
  //SERIAL COMMUNICATION
  Serial.begin(9600);
  //
  pinMode(capacitiveSensorPinInput, INPUT);
  pinMode(10, OUTPUT);

}

void loop() {
  // put your main code here, to run repeatedly:
  capacitiveSensorValue = digitalRead(capacitiveSensorPinInput);
  delay(10);
  //Serial.println(capacitiveSensorValue);

  //delay(15);
  // check if the current button state is different than the last state:
  if (capacitiveSensorValue != lastCapacitiveSensorValue) {

      Serial.println(capacitiveSensorValue);

    // do stuff if it is different here
    if (capacitiveSensorValue == HIGH) {
      //Serial.println(capacitiveSensorValue);

      //myservo.write(initialAngle);                  // sets the servo position according to the scaled value
      //Serial.write(capacitiveSensorValue);
      //delay(delayServo);
    }
    else{
            //Serial.println(capacitiveSensorValue);

       //Serial.println(capacitiveSensorValue);
       //Serial.write(capacitiveSensorValue);
    }


  }
  // save button state for next comparison:
  lastCapacitiveSensorValue = capacitiveSensorValue;
  
  //Serial.println(sensorValue);
}
