# -p:PublishTrimmed=true
dotnetArgs = -p:PublishSingleFile=true --no-self-contained -p:DebugType=None -c Release

all: build-all

build-all: clean create-dirs osx-x64 osx-arm64 win-x64 win-arm64 linux-x64 linux-arm64
	-mv dist/osx-x64/badengine-editor-backend dist/osx-x64/backend.exe
	-mv dist/osx-arm64/badengine-editor-backend dist/osx-arm64/backend.exe
	-mv dist/win-x64/badengine-editor-backend.exe dist/win-x64/backend.exe
	-mv dist/win-arm64/badengine-editor-backend.exe dist/win-arm64/backend.exe
	-mv dist/linux-x64/badengine-editor-backend dist/linux-x64/backend.exe
	-mv dist/linux-arm64/badengine-editor-backend dist/linux-arm64/backend.exe

prepare-front: build-all
	mv dist/osx-x64/* dist/darwin-x64.exe
	mv dist/osx-arm64/* dist/darwin-arm64.exe
	mv dist/win-x64/* dist/win32-x64.exe
	mv dist/win-arm64/* dist/win32-arm64.exe
	mv dist/linux-x64/* dist/linux-x64.exe
	mv dist/linux-arm64/* dist/linux-arm64.exe
	rm -rf dist/osx-x64
	rm -rf dist/osx-arm64
	rm -rf dist/win-x64
	rm -rf dist/win-arm64
	rm -rf dist/linux-x64
	rm -rf dist/linux-arm64
	-rm -rf ../badengine-editor/extraResources/backend/*
	cp -r dist/* ../badengine-editor/extraResources/backend/

create-dirs:
	-mkdir dist
	-mkdir dist/osx-x64
	-mkdir dist/osx-arm64
	-mkdir dist/win-x64
	-mkdir dist/win-arm64
	-mkdir dist/linux-x64
	-mkdir dist/linux-arm64

osx-x64:
	dotnet publish $(dotnetArgs) --runtime osx-x64 -o dist/osx-x64/

osx-arm64:
	dotnet publish $(dotnetArgs) --runtime osx-arm64 -o dist/osx-arm64/

win-x64:
	dotnet publish $(dotnetArgs) --runtime win-x64 -o dist/win-x64/

win-arm64:
	dotnet publish $(dotnetArgs) --runtime win-arm64 -o dist/win-arm64/

linux-x64:
	dotnet publish $(dotnetArgs) --runtime linux-x64 -o dist/linux-x64/

linux-arm64:
	dotnet publish $(dotnetArgs) --runtime linux-arm64 -o dist/linux-arm64/

clean:
	-rm -rf dist
