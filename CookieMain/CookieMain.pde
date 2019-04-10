import ddf.minim.*;
import processing.serial.*;
import lord_of_galaxy.timing_utils.*;
import oscP5.*;
import netP5.*;

Stopwatch timerToClose;//Declare
Stopwatch timerDeactivate;//Declare
Stopwatch spotTimer;//Declare

Minim minim;
AudioPlayer thunderSound;
AudioPlayer miracleSound;

OscP5 oscP5;
NetAddress myRemoteLocation;

String timerCallbackInfo = "";

OPC opc;
PImage im;
PImage im2;
PImage im3;
PImage im4;


Serial capacitivePort;  // Create object from Serial class
Serial spotPort;  // Create object from Serial class
Serial cookiePort;  // Create object from Serial class
boolean toggle = true;
String capacitiveTrigger;
int numberArduinos;
int time;
//WAAIT IN IN SECONDS
int timeToClose = 30;
int spotTimeInit = 2;
int spotTime = 15;
int deactivateTime = 0;
int deactivateTimeF = 55;


int imHeight;
float speed = 0.1;
float y; 
boolean breathing = true;
boolean thundering = false;
boolean insideCalm = true;
boolean spot = false;
boolean cookie = false;
boolean closeOnce = false;
boolean BreathOnce = false;
boolean debugLight = false;

void setup() 
{

  //CLOUD////
  size(1600, 200);
  minim = new Minim(this);
  thunderSound = minim.loadFile("thunder.mp3");
  miracleSound = minim.loadFile("opening.mp3");

  im = loadImage("cloud-breathing.jpg");
  im2 = loadImage("thunder2.jpg");  
  im3 = loadImage("inside.jpg");  
  im4 = loadImage("insideBlack.jpg");  

  //WE HAVE TO CHECK PORTS BEFORE EVERYTHING
  // Connect to the local instance of fcserver
  opc = new OPC(this, "127.0.0.1", 7890);
  opc.ledStrip(0, 64, 2*width/8, height/4, width/2 / 80.0, 0, false);
  opc.ledStrip(65, 64, 2*width/8, 2*height/4, width/2 / 80.0, 0, false);
  opc.ledStrip(129, 64, 6*width/8, height/4, width/2 / 80.0, 0, false);
  //opc.ledStrip(65, 66, width/2, 2*height/4, width/2 / 170.0, 0, false);
  //opc.ledStrip(129, 192, width/2, 3*height/4, width/2 / 170.0, 0, false);

  imHeight = im.height * width / im.width;
  //CLOUD END////

  numberArduinos = Serial.list().length;
  println(numberArduinos + " connected, ");
  println(Serial.list());

  //1231 - SPOTLIGHT
  //1221 - ACTUATOR
  //12441 - TRIGGER
  println("You have "+ numberArduinos + " arduinos connected");
  if (numberArduinos >= 1) {
    // if (Serial.list()[7] != null) {
    //String capallllcitiveSensor = Serial.list()[0]; //change the 0 to a 1 or 2 etc. to match your port
    //capacitivePort = new Serial(this, capacitiveSensor, 9600);
    //1221 - ACTUATOR
    // String cookieName = Serial.list()[7]; //change the 0 to a 1 or 2 etc. to match your port
    String cookieName = "/dev/tty.usbmodemFD1221";
    cookiePort = new Serial(this, cookieName, 9600);
    // } else {
    //   println("ACTUATOR IS NOT CONNECTED");
    //}
    //if (Serial.list()[8] != null) {
    //SPOTLIGHT
    //String cookieName = Serial.list()[1]; //change the 0 to a 1 or 2 etc. to match your port
    //cookiePort = new Serial(this, cookieName, 9600);
    //String spotLight = Serial.list()[8]; //change the 0 to a 1 or 2 etc. to match your port
    //String spotLight = "/dev/tty.usbmodem112";
    String spotLight = Serial.list()[6];
    spotPort = new Serial(this, spotLight, 9600);
    // } else {
    //    println("SPOTLIGHT IS NOT CONECTED");
    // }

    //if (Serial.list()[9] != null) {
    //TRIGGER
    //String cookieName = Serial.list()[7]; //change the 0 to a 1 or 2 etc. to match your port
    //cookiePort = new Serial(this, cookieName, 9600);
    //String capacitiveSensor = Serial.list()[9]; //change the 0 to a 1 or 2 etc. to match your port
    String capacitiveSensor = "/dev/tty.usbmodemFD12441";
    capacitivePort = new Serial(this, capacitiveSensor, 9600);
    //} else {
    //   println("TRIGGER IS NOT CONNECTED");
    // }
  } else {
    println("At least one of your Arduinos is not properly connected");
  }
  timerToClose = new Stopwatch(this);
  timerDeactivate = new Stopwatch(this);
  spotTimer = new Stopwatch(this);
  /* start oscP5, listening for incoming messages at port 12000 */
  oscP5 = new OscP5(this, 12000);
  myRemoteLocation = new NetAddress("127.0.0.1", 12000);
}
void draw() {
  ///TIMER
  //println(timerToClose.second());
  //println("Time: " + timerDeactivate.second() + ", Deactivate Time: " + deactivateTime);
  //println("Time: " + timerToClose.second() + ", Closing Time: " + timeToClose);

  if (timerDeactivate.second() >= deactivateTime && deactivateTime != 0 && !timerDeactivate.isPaused()) {
    println("TimerBreathing On");
    timerDeactivate.pause();
    cloudBreathingCue();
    spot = false;
    BreathOnce = true;
  }
  // if (!debugLight) {
  if (timerDeactivate.second() >= spotTimeInit && timerDeactivate.second() <= spotTimeInit + spotTime) {
    spot = true;
    openCookieCue();
    if ( miracleSound.position() == miracleSound.length() ) {
    }
    if (!miracleSound.isPlaying()) {  
      miracleSound.rewind();
      miracleSound.play();
    }
  } else {
    //spot = false;
  }
  //}
  if (timerToClose.second() > timeToClose && closeOnce) {
    if (cookiePort != null) {
      closeCookieCue  ();
      closeOnce = false;
    }
    println("Timer Finished");
    timerToClose.reset();
  }
  //TIMER
  y = (millis() * -speed) % imHeight;
  if (breathing) {
    image(im, 0, y, width/2, imHeight);
    image(im, 0, y + imHeight, width/2, imHeight);
  }
  if (thundering) {
    image(im2, 0, y, width/2, imHeight);
    image(im2, 0, y + imHeight, width/2, imHeight);
  }
  if (insideCalm) {
    image(im3, width/2, y, width/2, imHeight);
    image(im3, width/2, y + imHeight, width/2, imHeight);
  } else {
    image(im4, width/2, y, width/2, imHeight);
    image(im4, width/2, y + imHeight, width/2, imHeight);
  }
  //spotCue();
  //println(spot);
  if (!spot) {
    spotPort.write('0');
  } else {
    spotPort.write('1');
  }
  if ( capacitivePort != null) {
    if ( capacitivePort.available() > 0) 
    {  // If data is available,
      capacitiveTrigger = capacitivePort.readStringUntil('\n');         // read it and store it in val
      if (capacitiveTrigger != null) {  // If data is available,
        capacitiveTrigger = capacitiveTrigger.trim();
        //print(capacitiveTrigger); //print it out in the console
        if (capacitiveTrigger.equals("1") && timerDeactivate.second() >= deactivateTime) {
          //timerDeactivate.second() > deactivateTime
          cookieCue();
          //timerDeactivate.reset();
          //timerToClose.reset();


          //spotCue();
          println("Capacitor Touched");
          //thread("spotCue");
        } 

        //else if (capacitiveTrigger.equals("0")) {
        //  spotPort.write('0');         //send a 1
        //}
      }
    }
  }
}
void cookieCue() {
  //COOKIE CODE
  // 
  println("Timer started");
  if (cookiePort != null) {
    deactivateTime = deactivateTimeF;
    timerToClose.restart();
    timerDeactivate.restart();
    closeOnce = true;
    cloudThunderCue();
    //    openCookieCue();
    sendOSCmessage();
  } else {
    println("Cookie is not connected");
  }
}
void openCookieCue() {
  if (cookiePort != null) {
    cookiePort.write('1');
    println("Opening");
    cookiePort.write('0');
  }
}
void closeCookieCue() {
  if (cookiePort != null) {
    cookiePort.write('2');
    println("Closing");
    cookiePort.write('0');
  }
}
void stopCookieCue() {
  if (cookiePort != null) {
    cookiePort.write('3');
    println("StopCookie");
    cookiePort.write('0');
  }
}
void cloudBreathingCue() {
  println("Breathing");
  breathing = true;
  thundering = false;
  insideCalm = true;
}
void cloudThunderCue() {
  println("Thundering");
  thunderSound.rewind();
  thunderSound.play();
  breathing = false;
  thundering = true;
  insideCalm = false;
}

void spotCue() {
  if (spotPort != null) {
    //spotPort.write(1);
    //spotTimer.restart();
    //println("spot: 1");
  } else {
    println("Spot is not connected properly");
  }
}
void keyPressed() {
  switch(key) {  
    //TOGGLING SPOT
  case 'l':
    miracleSound.play();
    spot = !spot;
    break;
  case 'b':
    cloudBreathingCue();
    break;
  case 't':
    cloudThunderCue();
    break;
  case 'o':
    openCookieCue();
    break;
  case 'c':
    closeCookieCue();
    break;
  case 's':
    stopCookieCue();
    break;  
  case 'i':
    insideCalm = !insideCalm;
    break;
  case 'm':
    sendOSCmessage();
    break;
  }
}
void mousePressed() {
  cookieCue();
  //closeCookieCue();
}
void sendOSCmessage() {
  /* in the following different ways of creating osc messages are shown by example */
  OscMessage myMessage = new OscMessage("/message");
  myMessage.add(1); /* add an int to the osc message */
  /* send the message */
  oscP5.send(myMessage, myRemoteLocation);
}
