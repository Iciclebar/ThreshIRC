CSC=mcs

all: launcher.exe

launcher.exe: *.cs 
	$(CSC) /target:exe /r:../bin/Sharkbite.Thresher.dll /out:$@ $^

clean:
	-rm -f launcher.exe
