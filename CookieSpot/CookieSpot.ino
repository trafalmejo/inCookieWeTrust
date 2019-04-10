/*
  Fading

  This example shows how to fade an LED using the analogWrite() function.

  The circuit:
  - LED attached from digital pin 9 to ground.

  created 1 Nov 2008
  by David A. Mellis
  modified 30 Aug 2011
  by Tom Igoe

  This example code is in the public domain.

  http://www.arduino.cc/en/Tutorial/Fading
*/
char val; // Data received from the serial port
char lastVal; // Data received from the serial port

int velocity = 180;
int ledPin = 6;    // LED connected to digital pin 9
int testPin = 13;

void setup() {
  // nothing happens in setup
  Serial.begin(9600); // Start serial communication at 9600 bps
  // pinMode();
}

void loop() {
  // Serial.println("DOWN");
  //analogWrite(ledPin, 255);
   // analogWrite(ledPin, 255);

  if (Serial.available())
  { // If data is available to read,
    val = Serial.read(); // read it and store it in val
    // Serial.println("UP");
  }
  if (val != lastVal) {
    if (val == '1')
    { // If 1 was received
      FadeInCue();
      //analogWrite(ledPin, 0);
    } else if(val == '0') {
      FadeOutCue();
      //analogWrite(ledPin, 255);
    }
    Serial.println(val);
    delay(10); // Wait 10 milliseconds for next reading
  }
  lastVal = val;

}
void FadeInCue() {
  // fade in from min to max in increments of 5 points:
  for (int fadeValue = 20 ; fadeValue <= 255; fadeValue += 5) {
    // sets the value (range from 0 to 255):
    analogWrite(ledPin, fadeValue);
    // wait for 30 milliseconds to see the dimming effect
    delay(velocity);
  }
}
void FadeOutCue() {
  // fade out from max to min in increments of 5 points:
  for (int fadeValue = 255 ; fadeValue >= 20; fadeValue -= 5) {
    // sets the value (range from 0 to 255):
    analogWrite(ledPin, fadeValue);
    // wait for 30 milliseconds to see the dimming effect
    delay(velocity);
  }
}
