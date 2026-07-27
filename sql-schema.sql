CREATE TABLE countries (
	uuid VARCHAR(50) UNIQUE NOT NULL,
	name VARCHAR(100) NOT NULL,
	continent VARCHAR(100),
	sync_level INT NOT NULL
)