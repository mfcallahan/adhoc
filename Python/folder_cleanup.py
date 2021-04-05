#!/usr/bin/python3.8 python

# Delete all files in a folder which are older than the specified number days.
# Deploy:
# cp -u folder_cleanup.py /home/matt/Scripts
import os
import logging
import datetime
from types import SimpleNamespace

cleanupFolders = [
    SimpleNamespace(path = '/home/matt/Temp', days = 7, errors = []),
    SimpleNamespace(path = '/home/matt/Download', days = 30, errors = []),
    SimpleNamespace(path = '/home/matt/Desktop' ,days = 1, errors = [])
]

def main():
    # name the log file as "{this_filename}.log"
    logFileName = f'{os.path.splitext(os.path.basename(__file__))[0]}.log'
    configureLogger(logFileName)

    for folder in cleanupFolders:
        deleteFilesInFolder(folder)
        deleteEmptyFolders(folder)

        if len(folder.errors) > 0:
            for error in folder.errors:
                logging.error(error)

    logging.info(f'Folder cleanup complete at {datetime.datetime.now().strftime("%d-%m-%Y %H:%M:%S")}.')

# configure logging to write to file, overwriting the log file contents each time
def configureLogger(logFileName):
    logging.basicConfig(
        filename=logFileName,
        filemode='w',
        format='%(asctime)s,%(msecs)d %(name)s %(levelname)s %(message)s',
        datefmt='%m/%d/%Y %H:%M:%S',
        level=logging.DEBUG
    )

def deleteFilesInFolder(cleanupFolder):
    now = datetime.datetime.now()
    for root, dirs, files in os.walk(cleanupFolder.path):
        for file in files:
            currentFilePath = os.path.join(root, file)
            currentFileModifiedTime = datetime.datetime.fromtimestamp(os.path.getmtime(currentFilePath))
            
            if now - currentFileModifiedTime > datetime.timedelta(days=cleanupFolder.days):
                try:
                    os.remove(currentFilePath)
                except OSError as e:
                    cleanupFolder.errors.append(f'File "{currentFilePath}" - {e.strerror}')
                    

def deleteEmptyFolders(cleanupFolder):
    for root, dirs, files in os.walk(cleanupFolder.path, topdown = False):
        for dir in dirs:
            fullDirPath = os.path.join(root, dir)
            if os.path.isdir(fullDirPath) and len(os.listdir(fullDirPath)) == 0:
                try:
                    os.rmdir(fullDirPath)
                except OSError as e:
                    cleanupFolder.errors.append(f'Folder "{fullDirPath}" - {e.strerror}')

if __name__ == "__main__":
    main()