ALTER TABLE lesson_signal DROP COLUMN UserId;
ALTER TABLE lesson_signal ADD student_id VARCHAR(120) NOT NULL UNIQUE;
ALTER TABLE lesson_signal ADD CONSTRAINT constraint_student_id FOREIGN KEY (student_id)
	REFERENCES student(UserId)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT;