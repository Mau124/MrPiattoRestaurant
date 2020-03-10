	/* Data SHA1: a5142233b6b8474f300b213c0cb1d3c3813e58c3 */
	.file	"typemap.mj.inc"

	/* Mapping header */
	.section	.data.mj_typemap,"aw",@progbits
	.type	mj_typemap_header, @object
	.p2align	2
	.global	mj_typemap_header
mj_typemap_header:
	/* version */
	.long	1
	/* entry-count */
	.long	1214
	/* entry-length */
	.long	262
	/* value-offset */
	.long	145
	.size	mj_typemap_header, 16

	/* Mapping data */
	.type	mj_typemap, @object
	.global	mj_typemap
mj_typemap:
	.size	mj_typemap, 318069
	.include	"typemap.mj.inc"
