# Automated-Text-Sending-Application-Using-GSM_Module
GSM Module connected with a UART to TTL converter. Application made in C#. I made this application for it's purpose to alert registered users the rise of water level from an ultrasonic sensor with two gsm modules.

note: This system runs on a local made database on PhpMyAdmin.

Details:
Text comes from another gsm module that is connected to a microcontroller and an ultrasonic sensor (measures water level height) each height will send a string text to the gsm module connected to the application. Hence once a text is received, the program will send an alert text to all numbers registered in the database.
