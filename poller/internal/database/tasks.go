package database

import (
	"database/sql"

	"github.com/google/logger"
)

// Task represents a task that needs to be finished based on its finished boolean
type Task struct {
	PollerName string
	Finished   bool
}

func GetPendingTasks(db *sql.DB) []Task {
	unfinishedTasksQuery := "SELECT name, finished FROM tasks WHERE finished = $1"
	unfinished := 0
	rows, err := db.Query(unfinishedTasksQuery, unfinished)
	if err != nil {
		logger.Errorf("query \"%s\" with argument(s) \"%v\" failed with error:\n%v", unfinishedTasksQuery, unfinished, err)
	}
	defer rows.Close()

	tasks := make([]Task, 10)
	for rows.Next() {
		var task Task
		err := rows.Scan(&task)
		if err != nil {
			logger.Errorf("could not read database result into struct %v\n%v", task, err)
		}
		tasks = append(tasks, task)
	}
	err = rows.Err()

	if err != nil {
		logger.Errorf("encountered error when iteration through rows\n%v", err)
	}
	return tasks
}
