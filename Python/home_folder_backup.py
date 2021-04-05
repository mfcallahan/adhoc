# Copy all files from a specified source folder in the Home directory to a specified destination folder for backup
# purposes. If a file exists in the destination folder but not the source folder, it is deleted.

import os
import logging
import glob
import datetime
import fnmatch
from datetime import datetime
from types import SimpleNamespace
from shutil import copyfile
from pathlib import Path

backupPath = '/mnt/XtraDisk/OneDrive/Mint_Backup'

backupFolders = [
    SimpleNamespace(sourcePath = '/home/matt/Documents', errors = []),
    SimpleNamespace(sourcePath = '/home/matt/Pictures', errors = [])
]

def main():
    # name the log file as "{this_filename}.log"
    logFileName = f'{os.path.splitext(os.path.basename(__file__))[0]}.log'
    configureLogger(logFileName)

    for folder in backupFolders:
        syncFolders(folder)

        # log each error to file
        [logging.error(error) for error in folder.errors if len(folder.errors) > 0]

    logging.info(f'Folder backup complete at {datetime.now().strftime("%d-%m-%Y %H:%M:%S")}.')

# configure logging to write to file, overwriting the log file contents each time
def configureLogger(logFileName):
    logging.basicConfig(
        filename=logFileName,
        filemode='w',
        format='%(asctime)s,%(msecs)d %(name)s %(levelname)s %(message)s',
        datefmt='%m/%d/%Y %H:%M:%S',
        level=logging.DEBUG
    )
 
def syncFolders(backupFolder):
    sourceFiles = getAllFilesInFolder(backupFolder.sourcePath)
    destFiles = getAllFilesInFolder(backupFolder.destPath)

    # delete any files in the destination folder which do not exist in the source folder
    for file in destFiles:
        if (sourceFiles.count(file.replace(backupFolder.destPath, '~')) == 0):
            try:
                os.remove(file)
            except OSError as e:
                backupFolder.errors.append(f'File "{file}" - {e.strerror}')
            
    # delete any empty folders left inside the destination folder
    try:
        deleteEmptyFolders(backupFolder.destPath)
    except OSError as e:
        backupFolder.errors.append(f'deleteEmptyFolders({backupFolder.destPath}) error: {e.strerror}')
    
    # copy all files from source folder to destination folder
    for sourceFile in sourceFiles:
        destFile = os.path.join(backupFolder.destPath , os.path.basename(os.path.normpath(backupFolder.sourcePath)))
        try:
            os.makedirs(os.path.dirname(destFile), exist_ok=True)
            copyfile(sourceFile, destFile)
        except OSError as e:
            backupFolder.errors.append(f'Error copying "{sourceFile}" to "{destFile}" - {e.strerror}')

def getAllFilesInFolder(folder):
    files = []
    for file in Path(folder).glob('**/*.*'):
        files.append(str(file))
    
    return files

def deleteEmptyFolders(folder):
    for root, dirs, files in os.walk(folder, topdown = False):
        for dir in dirs:
            fullDirPath = os.path.join(root, dir)
            if os.path.basename(os.path.normpath(fullDirPath)) == folder:
                continue
            if os.path.isdir(fullDirPath) and len(os.listdir(fullDirPath)) == 0:
                os.rmdir(fullDirPath) 

if __name__ == "__main__":
    main()
