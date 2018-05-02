ALTER TABLE lesson_signal DROP COLUMN UserId;
ALTER TABLE lesson_signal ADD student_id INT(20) NOT NULL;
ALTER TABLE lesson_signal ADD CONSTRAINT constraint_student_id FOREIGN KEY (student_id)
	REFERENCES student(Id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT;