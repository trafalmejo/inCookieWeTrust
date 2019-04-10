char val; // Data received from the serial port
char lastVal; // Data received from the serial port

//motor A connected between A01 and A02
//motor B connected between B01 and B02

int STBY = 10; //standbyA

//Motor A
int PWMA = 3; //Speed control
int AIN1 = 9; //Direction
int AIN2 = 8; //Direction

//Motor B
//int PWMB = 5; //Speed control
//int BIN1 = 11; //Direction
//int BIN2 = 12; //Direction

// SWITCH
const int standbyA = 5;
const int standbyB = 11;
const int statePin = 6;

int switchStateA = 0;
int switchStateB = 0;
int stateState = 0;
int lastswitchStateA = 0;
int lastswitchStateB = 0;
int laststateState = 0;

int moveUp = 0;
bool standby = true;
bool opening = false;
bool closing = false;
int state = 0;

void setup() {
  Serial.begin(9600);
  pinMode(STBY, OUTPUT);

  pinMode(PWMA, OUTPUT);
  pinMode(AIN1, OUTPUT);
  pinMode(AIN2, OUTPUT);

  pinMode(standbyA, INPUT_PULLUP);
  pinMode(standbyB, INPUT_PULLUP);
  pinMode(statePin, INPUT_PULLUP);
  //
  //pinMode(PWMB, OUTPUT);
  //pinMode(BIN1, OUTPUT);
  //pinMode(BIN2, OUTPUT);
}

void loop() {

  switchStateA = digitalRead(standbyA);
  switchStateB = digitalRead(standbyB);
  stateState = digitalRead(statePin );

  //Serial.println("Switch A :" + switchStateA);
  //Serial.println("Switch B :" + switchStateB);

  if (standby) {
    stop();
    Serial.println("standby..");
  }
  if (opening) {
    move(1, 255, 1); //motor 1, full speed, left
    Serial.println("opening..");
  }
  if (closing) {
    move(1, 255, 0); //motor 1, full speed, left
    Serial.println("closing..");
  }

  if (switchStateA != lastswitchStateA ) {
    //THERE IS CHANGE
    if (switchStateA == LOW ) {
      standby = true;
      opening = false;
      closing = false;
      state = 0;
      Serial.println("stopping..");
    }
  }
  lastswitchStateA =  switchStateA;

  if (switchStateB != lastswitchStateB ) {
    //THERE IS CHANGE
    if (switchStateB == LOW ) {
      standby = true;
      opening = false;
      closing = false;
      state = 0;
      Serial.println("stopping..");
    }
  }
  lastswitchStateB =  switchStateB;


  if (stateState != laststateState ) {
    //THERE IS CHANGE
    //THIS BUTTON SIMULATES TRIGGER
    if (stateState == LOW ) {
      state++;
      if (state % 3 == 0) {
        standby = true;
        opening = false;
        closing = false;
      }
      else if (state % 3 == 1) {
        standby = false;
        opening = true;
        closing = false;
      }
      else if (state % 3 == 2) {
        standby = false;
        opening = false;
        closing = true;
      }
      Serial.println("ButtonB");
    }
  }
  laststateState =  stateState;

  if (Serial.available())
  { // If data is available to read,
    val = Serial.read(); // read it and store it in val
    //Serial.println("UP");
  }
  if (val != lastVal) {
    //1 IS FOR OPENING
    if (val == '1')
    { // If 1 was received
      standby = false;
      opening = true;
      closing = false;
      //2 IS FOR CLOSING
    } else if (val == '2') {
      standby = false;
      opening = false;
      closing = true;
    }
    //3 IS FOR STANDBY
    else if (val == '3') {
      standby = true;
      opening = false;
      closing = false;
    }
    Serial.println(val);
    delay(10); // Wait 10 milliseconds for next reading
  }
  lastVal = val;
}

void move(int motor, int speed, int direction) {
  //Move specific motor at speed and direction
  //motor: 0 for B 1 for A
  //speed: 0 is off, and 255 is full speed
  //direction: 0 clockwise, 1 counter-clockwise

  digitalWrite(STBY, HIGH); //disable standbyA

  boolean inPin1 = LOW;
  boolean inPin2 = HIGH;

  if (direction == 1) {
    inPin1 = HIGH;
    inPin2 = LOW;
  }

  if (motor == 1) {
    digitalWrite(AIN1, inPin1);
    digitalWrite(AIN2, inPin2);
    analogWrite(PWMA, speed);
  }

}

void stop() {
  //enable standbyA
  //digitalWrite(STBY, LOW);
  digitalWrite(AIN1, LOW);
  digitalWrite(AIN2, LOW);
  //delay(5000);
}
