sudo apt-get install -y nginx
sudo systemctl start nginx

sudo apt-get install bind9 -y
sudo mv /etc/bind/named.conf.options /etc/bind/named.conf.options.bak
sudo wget https://raw.githubusercontent.com/xingyang-li/SwiftTestFramework/main/dnsExtension/named.conf.options.txt
sudo mv named.conf.options.txt /etc/bind/named.conf.options
sudo wget https://raw.githubusercontent.com/xingyang-li/SwiftTestFramework/main/dnsExtension/named.conf.local.txt
sudo mv named.conf.local.txt /etc/bind/named.conf.local
sudo wget https://raw.githubusercontent.com/xingyang-li/SwiftTestFramework/main/dnsExtension/db.example.com.txt
sudo mv stf.db.example.com.txt /etc/bind/db.example.com
sudo service bind9 restart


echo "Worked" > marker.txt