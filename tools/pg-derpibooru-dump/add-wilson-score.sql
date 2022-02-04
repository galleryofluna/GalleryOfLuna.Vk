ALTER TABLE images
	ADD COLUMN wilson_score NUMERIC;
	
CREATE INDEX images_IX_wilson_score ON images (wilson_score);

UPDATE images
	SET wilson_score = ((upvotes / ((upvotes + downvotes) / 1)) + 6.634900189 / (2 * ((upvotes + downvotes) / 1)) - 2.57583 * sqrt(((upvotes / ((upvotes + downvotes) / 1)) * (1 - (upvotes / ((upvotes + downvotes) / 1))) + 6.634900189 / (4 * ((upvotes + downvotes) / 1))) / ((upvotes + downvotes) / 1))) / (1 + 6.634900189 / ((upvotes + downvotes) / 1))
	WHERE upvotes + downvotes <> 0;