CSC=mcs

all: Sharkbite.Thresher.dll

Sharkbite.Thresher.dll: source/*.cs source/Dcc/*.cs source/Ctcp/*.cs
	$(CSC) /target:library /out:$@ $^

clean:
	-rm -f Sharkbite.Thresher.dll
