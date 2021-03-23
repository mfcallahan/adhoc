import os
import time
import datetime

class CleanupFolder:
    def __init__(self, path, days):
        self.path = path
        self.days = days

def main():
    cleanupFolders = [
        CleanupFolder('/home/matt/Temp', 7),
        CleanupFolder('/home/matt/Download', 30)
    ]

    for folder in cleanupFolders:
        deleteFilesInFolder(folder.path, folder.days)
        deleteEmptyFolders(folder.path)

def deleteFilesInFolder(path, days):
    for root, dirs, files in os.walk(path):
        for file in files:
            currentFilePath = os.path.join(root, file)
            currentFileModifiedTime = datetime.datetime.fromtimestamp(os.path.getmtime(currentFilePath))
            
            if datetime.datetime.now() - currentFileModifiedTime > datetime.timedelta(days=days):
                os.remove(currentFilePath)
                #print('remove(' + currentFilePath + ')')

def deleteEmptyFolders(path):
    for root, dirs, files in os.walk(path):
        if len(os.listdir(root)) == 0:
            os.rmdir(root)
            #print('remove(' + root + ')')

if __name__ == "__main__":
    main()
