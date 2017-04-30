
import serial
import commands

ser = serial.Serial('/dev/ttyS0') 

line = ''
ser.write('> ')

while True:
	char = ser.read()

	if (ord(char) == 13):

		ser.write('\n')

		if (len(line) > 0): 
			# output
			output = commands.getoutput(line)
			ser.write(output + '\n')
			line = ''

		ser.write('> ')

	elif (ord(char) == 8):

		if (len(line) > 0): 
			line = line[:-1]

	else:
		# echo
		ser.write(char)
		line = line + char

