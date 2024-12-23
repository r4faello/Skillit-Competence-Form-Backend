﻿// <auto-generated />
using System;
using CompetenceForm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompetenceForm.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241223095931_AdjustedModels")]
    partial class AdjustedModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompetenceForm.Models.Answer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InpactOnCompetence")
                        .HasColumnType("int");

                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("CompetenceForm.Models.Competence", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompetenceSetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompetenceSetId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Competences");
                });

            modelBuilder.Entity("CompetenceForm.Models.CompetenceSet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("CompetenceSets");
                });

            modelBuilder.Entity("CompetenceForm.Models.CompetenceValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompetenceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubmittedRecordId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompetenceId");

                    b.HasIndex("SubmittedRecordId");

                    b.ToTable("CompetenceValues");
                });

            modelBuilder.Entity("CompetenceForm.Models.Draft", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompetenceSetId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("InitiatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CompetenceSetId");

                    b.ToTable("Drafts");
                });

            modelBuilder.Entity("CompetenceForm.Models.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("CompetenceForm.Models.QuestionAnswer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnswerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DraftId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("DraftId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionAnswerPairs");
                });

            modelBuilder.Entity("CompetenceForm.Models.SubmittedRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompetenceSetId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("SubmittedRecords");
                });

            modelBuilder.Entity("CompetenceForm.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CompetenceForm.Models.Answer", b =>
                {
                    b.HasOne("CompetenceForm.Models.Question", null)
                        .WithMany("AnswerOptions")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("CompetenceForm.Models.Competence", b =>
                {
                    b.HasOne("CompetenceForm.Models.CompetenceSet", null)
                        .WithMany("Competences")
                        .HasForeignKey("CompetenceSetId");

                    b.HasOne("CompetenceForm.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("CompetenceForm.Models.CompetenceValue", b =>
                {
                    b.HasOne("CompetenceForm.Models.Competence", "Competence")
                        .WithMany()
                        .HasForeignKey("CompetenceId");

                    b.HasOne("CompetenceForm.Models.SubmittedRecord", null)
                        .WithMany("CompetenceValues")
                        .HasForeignKey("SubmittedRecordId");

                    b.Navigation("Competence");
                });

            modelBuilder.Entity("CompetenceForm.Models.Draft", b =>
                {
                    b.HasOne("CompetenceForm.Models.User", "Author")
                        .WithMany("Drafts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompetenceForm.Models.CompetenceSet", "CompetenceSet")
                        .WithMany()
                        .HasForeignKey("CompetenceSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("CompetenceSet");
                });

            modelBuilder.Entity("CompetenceForm.Models.QuestionAnswer", b =>
                {
                    b.HasOne("CompetenceForm.Models.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompetenceForm.Models.Draft", null)
                        .WithMany("QuestionAnswerPairs")
                        .HasForeignKey("DraftId");

                    b.HasOne("CompetenceForm.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Answer");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("CompetenceForm.Models.SubmittedRecord", b =>
                {
                    b.HasOne("CompetenceForm.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("CompetenceForm.Models.CompetenceSet", b =>
                {
                    b.Navigation("Competences");
                });

            modelBuilder.Entity("CompetenceForm.Models.Draft", b =>
                {
                    b.Navigation("QuestionAnswerPairs");
                });

            modelBuilder.Entity("CompetenceForm.Models.Question", b =>
                {
                    b.Navigation("AnswerOptions");
                });

            modelBuilder.Entity("CompetenceForm.Models.SubmittedRecord", b =>
                {
                    b.Navigation("CompetenceValues");
                });

            modelBuilder.Entity("CompetenceForm.Models.User", b =>
                {
                    b.Navigation("Drafts");
                });
#pragma warning restore 612, 618
        }
    }
}
