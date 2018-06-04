USE ucubot;
CREATE VIEW student_signals 
	AS SELECT student.FirstName, student.LastName, 
	(CASE lesson_signal.SignalType WHEN -1 THEN "Simple" WHEN 0 THEN "Normal" WHEN 1 THEN "Hard" END) 
	AS SignalType, count(lesson_signal.student_id) 
	AS COUNT FROM student 
	JOIN (lesson_signal) ON (lesson_signal.student_id = student.Id) 
	GROUP BY lesson_signal.SignalType, student.UserId;