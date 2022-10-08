# wireshark-esk
C# program that automates packet sniffing using native windows pktmon
# description
This program MUST be ran with elevation since it utilizes both netstat -ab and pktmon commands
The program with get all ports with an established connection on your machine then proceed to recieve the incoming packets.  
Every process will have a text file along with it so you can look at your ports, the mac-address of all connected devices on your network, all ipv4 and ipv6 addresses
Since this is a work in progress there is no need to bash it quite yet.  It will be optimized later aswell as include more functionality for specific needs.
