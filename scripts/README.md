# Agile Retrospective


note: these scripts assume that the project is located at directory named /projects/agile-retro.


### files in scripts directory

* **Dockerfile** - This is the file which defines how the image should be created
* **docker-build** - This script builds the containter image named dotnetdb-image.   The running container is named dotnetdb and can be listed with the command `docker ps`
* **docker-run** - This script starts the container and the database.   This will occupy the terminal until it is closed with database messages.  Start a new terminal and execute `run-more` to get access to a new terminal
* **docker-run-more** - This script starts a second (or third ...) terminal into the already running container.
* **docker-start** - This command is executed by the run script in the container when it is started.  It should not be necessary to run this separately


### URLs

[http://localhost:5000/notes] - Launches the UI

