
//#include "GamepadStatus.h"

int sensorPin = A5;    // select the input pin for the potentiometer
int ledPin = 13;      // select the pin for the LED
int sensorValue = 0;  // variable to store the value coming from the sensor
int rgbLedPins[] = {3,5,6,9,10,11};
int switches[] = {2,4,7,8,12,13};
// 2 4 7 8 12 13

bool btn1, btn2, btn3;
bool switch1, switch2, switch3;
double sliderLeft, sliderRight;
double pot1, pot2, pot3;

void setup() {
  // Leds
  for (int i = 0; i < 6; i++) {
    int pin = rgbLedPins[i];
    pinMode(pin, OUTPUT);
  }
  // Switches input
  for (int i = 0; i < 6; i++) {
    int pin = switches[i];
    pinMode(pin, INPUT);
  }
  
  Serial.begin(9600);      // open the serial port at 9600 bps:   
}

unsigned long last_ts = 0;

void fillStatWithBtn() {
  btn1 = digitalRead(8) == 0 ? false : true;
  btn2 = digitalRead(12) == 0 ? false : true;
  btn3 = digitalRead(13) == 0 ? false : true;
}

// invert switches
void fillStatWithSwitches() {
  switch3 = digitalRead(2) == 0 ? true : false;
  switch2 = digitalRead(4) == 0 ? true : false;
  switch1 = digitalRead(7) == 0 ? true : false;
}

// sliders are not linear
void fillStatWithPotsNSliders() {
  sliderLeft = analogRead(A5) / 1023.0;
  sliderRight = analogRead(A4) / 1023.0;
  
  fixSlider(&sliderLeft);
  fixSlider(&sliderRight);
  pot1 = analogRead(A0) / 1023.0;
  pot2 = analogRead(A1) / 1023.0;
  pot3 = analogRead(A2) / 1023.0;
}

void fixSlider(double* value) {
  if (*value < 0.03) *value=0;
  if (*value > 0.96) *value=1;
}

void adjustLights() {
  int r1 =0,g1 =0,b1 =0, r2=0,g2=0,b2=0;
  r1 = r2 = sliderLeft * 255;
  b1 = b2 = sliderRight * 255;
  g1 = g2 = pot2 * 255;
  
  if (btn1) b1 = 255;
  else if (btn2) g1 = 255;
  else if (btn3) r1 = 255;
  
  showLightColor(0, r1, g1, b1);
  showLightColor(1, r2, g2, b2);
}

void printAsJson() {
  Serial.print(btn1 ? "1" : "0");
  Serial.print(",");
  Serial.print(btn2 ? "1" : "0");
  Serial.print(",");
  Serial.print(btn3 ? "1" : "0");
  Serial.print(",");
  Serial.print(switch1 ? "1" : "0");
  Serial.print(",");
  Serial.print(switch2 ? "1" : "0");
  Serial.print(",");
  Serial.print(switch3 ? "1" : "0");
  Serial.print(",");
  Serial.print((int)(sliderLeft*100));
  Serial.print(",");
  Serial.print((int)(sliderRight*100));
  Serial.print(",");
  Serial.print((int)(pot1*100));
  Serial.print(",");
  Serial.print((int)(pot2*100));
  Serial.print(",");
  Serial.println((int)(pot3*100));
//  Serial.println("}");
}

void loop() {
  // Make gamepad status
  fillStatWithBtn();
  fillStatWithSwitches();
  fillStatWithPotsNSliders();
  
  adjustLights();
  printAsJson();
  /*Serial.print(" Button statuses : ");
  for (int i = 0; i < 6; i++) {
    int pin = switches[i];
    int stat = digitalRead(pin);
      Serial.print(stat);  
      Serial.print(" ");
  }*/
  //showLightColor(0, 255,0,0);
  //showLightColor(1, 255,0,0);
  //showLightColor(0, 0,255,0);
  //
  //showLightColor(1, 0,255,0);
  //showLightColor(0, 0,0,255);
//  showLightColor(1, 0,0,255);
  //rgbTest();
  // read the value from the sensor:
  /*sensorValue = analogRead(sensorPin);    
  // turn the ledPin on
  digitalWrite(ledPin, HIGH);  
  // stop the program for <sensorValue> milliseconds:
  delay(sensorValue);          
  // turn the ledPin off:        
  digitalWrite(ledPin, LOW);   
  // stop the program for for <sensorValue> milliseconds:
  unsigned long now = millis();
  if (now - last_ts > 300) {
      last_ts = now;
      Serial.print(/*linearize(sensorValue);
      Serial.print("\n");
  }*/
  delay(1);                
}

static const int pinR1 = 9, pinG1 = 10, pinB1 = 11;
static const int pinR2 = 3, pinG2 = 5, pinB2 = 6;
void showLightColor(int ledTargeted, byte R, byte G, byte B) {
  if (ledTargeted == 0) {
    analogWrite(pinR1, R);
    analogWrite(pinB1, B);
    analogWrite(pinG1, G);
  }
  else {
    analogWrite(pinR2, R);
    analogWrite(pinB2, B);
    analogWrite(pinG2, G);
  }
}

void rgbTest() {
  unsigned long now = millis();
  unsigned long mod = now%8000;
  unsigned long part = mod/1000;
  unsigned long power = mod%1000*255/1000;
  if (part == 0) {
       analogWrite(pinR1, power);
       analogWrite(pinR2, power);
  }
  else if (part == 1) {
       analogWrite(pinR1, 255-power);
       analogWrite(pinR2, 255-power);
  }
  else if (part == 2) {
       analogWrite(pinG1, power);
       analogWrite(pinG2, power);
  }
  else if (part == 3) {
       analogWrite(pinG1, 255-power);
       analogWrite(pinG2, 255-power);
  }
  else if (part == 4) {
       analogWrite(pinB1, power);
       analogWrite(pinB2, power);
  }
  else if (part == 5) {
       analogWrite(pinB1, 255-power);
       analogWrite(pinB2, 255-power);
  }
  else if (part == 6) {
       analogWrite(pinR1, power);
       analogWrite(pinR2, power);
       analogWrite(pinB1, power);
       analogWrite(pinB2, power);
       analogWrite(pinG1, power);
       analogWrite(pinG2, power);
  }
  else if (part == 7) {
       power = 255-power;
       analogWrite(pinR1, power);
       analogWrite(pinR2, power);
       analogWrite(pinB1, power);
       analogWrite(pinB2, power);
       analogWrite(pinG1, power);
       analogWrite(pinG2, power);
  }
}
