import pyautogui as PAG
import socket
import time
import threading

PORT =999

S=socket.socket()
S.bind(("0.0.0.0",PORT));
S.listen(3)
class events:
	def __init__(self,l):
		try:
			self.dic[l[0]](l[1:])
		except:
			print("##############")
			print("bad input")
			print(l)
			print("##############")
			print()
			return
	class input:
		def __init__(self,l):
			self.dic[l[0]](l[1:])
		class mouse:
			def __init__(self,l):
				self.dic[l[0]](l[1:])
			def move(p):
				try:
					if(len(p)!=1):
						raise Exception("1")
					a=p[0].replace("[","").replace("]","").split(",")
					if(len(a)!=2):
						raise Exception("2")
					x,y=float(a[0]),float(a[1])
					x,y=int(x),int(y)
				except Exception as e:
					print("##############")
					print("bad input")
					print(e)
					print("MOUSE_MOVE got",p)
					print("##############")
					print()
					return
				#bc PAG's (0,0) is Left-Top corner
				# t=time.time()
				# PAG.move(x,-y)
				threading.Thread(target=PAG.move, args=(x,-y) ).start()
				# print("moved in:",time.time()-t)
			def Lclick(p):
				PAG.click()
			def Rclick(p):
				PAG.click(button='right')
			dic={"MOVE":move,"RCLICK":Rclick,"LCLICK":Lclick}
		dic={"MOUSE":mouse}
	class show:
		def __init__(self,l):
			print("_".join(l))
	dic={"INPUT":input,"SHOW":show}

# events.input(input().split("_"))
PAG.FAILSAFE=False
while True:
	con,addr=S.accept()
	print("Connected ",addr)
	while True:
		data=con.recv(1024)
		if(not data):
			break
		for packet in data.split(b"|||")[:-1]:
			ev=""
			try:
				ev=packet.decode("utf-8")
			except:
				print("got:".packet)
				continue
			# t=time.time()
			events(ev.split("_"))
			# print("Took:",time.time()-t)
	print("Disconnected")
# MOUSE_MOVE_[1.2,3.4]
