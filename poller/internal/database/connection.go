package database

import (
	"database/sql"
	"fmt"
	"log"

	// Sql driver has blank import
	_ "github.com/lib/pq"
)

const databaseType = "postgres"
const databaseUsername = "david"
const databaseName = "maximiz"
const sslMode = "disable"

// ConnectToDatabase connects to the database
func ConnectToDatabase() (db *sql.DB) {
	connStr := fmt.Sprintf("user=%s dbname=%s sslmode=%s", databaseUsername, databaseName, sslMode)
	db, err := sql.Open(databaseType, connStr)
	if err != nil {
		log.Fatal(err)
	}
	return db
}


